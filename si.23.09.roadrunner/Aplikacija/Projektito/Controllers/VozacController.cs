    using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace Projektito.Controllers;

[ApiController]
[Route("[controller]")]
public class VozacController : ControllerBase
{
   public Context Context { get; set; }
  public VozacController(Context context)
  {
        Context=context;
  }
  [AllowAnonymous]
  [Route("AddVozac")]
  [HttpPost]
  public async Task<IActionResult> AddVozac ([FromBody]Vozac Vozac)
  {
    try
    {
        if(Vozac.Ime.Length<3 || Vozac.Ime.Length>30 || Regex.IsMatch(Vozac.Ime,"^[a-zA-z]+$")==false)
            ModelState.AddModelError("Ime", "Ime treba da ima između 3 i 30 karaktera i sadrži samo slovne karaktere.");
        if(Vozac.Prezime.Length<3 || Vozac.Prezime.Length>30 || Regex.IsMatch(Vozac.Prezime,"^[a-zA-z]+$")==false)
            ModelState.AddModelError("Prezime", "Prezime treba da ima između 3 i 30 karaktera i sadrži samo slovne karaktere.");
        if(Vozac.JMBG.Length!= 13 || Regex.IsMatch(Vozac.JMBG,"^[0-9]+$")==false)
            ModelState.AddModelError("JMBG", "JMBG treba da ima tačno 13 cifara.");
        if(Vozac.Email.Length<6 || Vozac.Email.Length>30 || Regex.IsMatch(Vozac.Email,@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")==false)
            ModelState.AddModelError("Email", "Email treba da bude između 6 i 30 karaktera i u validnom formatu.");
        if(Vozac.KorisnickoIme.Length>20 || Vozac.KorisnickoIme.Length<1 || Regex.IsMatch(Vozac.KorisnickoIme,"^[a-zA-Z][a-zA-Z0-9]*$")==false)
            ModelState.AddModelError("KorisnickoIme", "Korisničko ime treba da ima između 1 i 20 karaktera i može sadržati samo slovne karaktere i brojeve.");
        if(Vozac.Sifra.Length>20 || Vozac.Sifra.Length<1 || Regex.IsMatch(Vozac.Sifra,"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#!@$^&*-]).{8,}$")==false)
            ModelState.AddModelError("Sifra", "Sifra mora da ima jedno veliko,jedno malo slovo, jedan specijalni znak i najmanja duzina je 8 karaktera");
        if(Regex.IsMatch(Vozac.BrojTelefona,@"^\+?[0-9][0-9\s.-]{7,11}$")==false)
            ModelState.AddModelError("BrojTelefona","Broj mora da se sastoji samo od cifara i mora da ih bude od 7 do 11");
        if (!ModelState.IsValid)
                return BadRequest(ModelState);
        string sifra= BCrypt.Net.BCrypt.HashPassword(Vozac.Sifra,10);
        Vozac.Sifra=sifra;    
        var KompanijaEmail = Context.Kompanija!.Where( p => p.Email == Vozac.Email).FirstOrDefault();
        var KompanijaUsername = Context.Kompanija!.Where( p => p.KorisnickoIme == Vozac.KorisnickoIme).FirstOrDefault();
        var VozacEmail = Context.Vozac!.Where(p => p.Email == Vozac.Email).FirstOrDefault();
        var VozacUsername = Context.Vozac!.Where(p=>p.KorisnickoIme == Vozac.KorisnickoIme).FirstOrDefault();
        var DispecerEmail = Context.Dispecer!.Where(p=>p.Email == Vozac.Email).FirstOrDefault();
        var DispecerUsername = Context.Dispecer!.Where(p=>p.KorisnickoIme == Vozac.KorisnickoIme).FirstOrDefault();
        if(KompanijaEmail!=null || VozacEmail!=null || DispecerEmail!=null)
            return BadRequest("Vec postoji nalog sa tim emailom");
        if(KompanijaUsername!= null || VozacUsername!= null || DispecerUsername!= null)
            return BadRequest("Vec postoji nalog sa tim korisnickim imenom");
        Context.Vozac!.Add(Vozac);
        await Context.SaveChangesAsync();
        return Ok("Uspesno dodat Vozac");
        
    }
    catch(Exception ex){
        return BadRequest(ex.Message);
    }
  }
  [Authorize(Roles ="Vozac,Dispecer")]
  [Route("UpdateVozac/{id}")]
  [HttpPut]
  public async Task<IActionResult> UpdateVozac([FromBody]Vozac Vozac, int id)
  {
      try
      {
        var Voz = Context.Vozac!.Find(id);
        if(Voz!=null)
        {
            if(Vozac.Ime.Length<3 || Vozac.Ime.Length>30 || Regex.IsMatch(Vozac.Ime,"^[a-zA-z]+$")==false)
                ModelState.AddModelError("Ime", "Ime treba da ima između 3 i 30 karaktera i sadrži samo slovne karaktere.");
            if(Vozac.Prezime.Length<3 || Vozac.Prezime.Length>30 || Regex.IsMatch(Vozac.Prezime,"^[a-zA-z]+$")==false)
                ModelState.AddModelError("Prezime", "Prezime treba da ima između 3 i 30 karaktera i sadrži samo slovne karaktere.");
            if(Vozac.JMBG.Length!= 13 || Regex.IsMatch(Vozac.JMBG,"^[0-9]+$")==false)
                ModelState.AddModelError("JMBG", "JMBG treba da ima tačno 13 cifara.");
            if(Vozac.Email.Length<6 || Vozac.Email.Length>30 || Regex.IsMatch(Vozac.Email,@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")==false)
                ModelState.AddModelError("Email", "Email treba da bude između 6 i 30 karaktera i u validnom formatu.");
            if(Vozac.KorisnickoIme.Length>20 || Vozac.KorisnickoIme.Length<1 || Regex.IsMatch(Vozac.KorisnickoIme,"^[a-zA-Z][a-zA-Z0-9]*$")==false)
                ModelState.AddModelError("KorisnickoIme", "Korisničko ime treba da ima između 1 i 20 karaktera i može sadržati samo slovne karaktere i brojeve.");
            if(Regex.IsMatch(Vozac.BrojTelefona,@"^\+?[0-9][0-9\s.-]{7,11}$")==false)
                ModelState.AddModelError("BrojTelefona","Broj mora da se sastoji samo od cifara i mora da ih bude od 7 do 11");
            if (!ModelState.IsValid)
                    return BadRequest(ModelState);
            if(Vozac.Email != Voz.Email)
                {
                    var KompanijaEmail = Context.Kompanija!.Where( p => p.Email == Vozac.Email).FirstOrDefault();
                    var VozacEmail = Context.Vozac.Where(p => p.Email == Vozac.Email).FirstOrDefault();
                    var DispecerEmail = Context.Dispecer!.Where(p=>p.Email == Vozac.Email).FirstOrDefault();
                    if(KompanijaEmail!=null || VozacEmail!=null || DispecerEmail!=null)
                        return BadRequest("Vec postoji nalog sa tim emailom");
                }
            else if ( Vozac.KorisnickoIme!= Voz.KorisnickoIme)
                {
                    var VozacUsername = Context.Vozac.Where(p=>p.KorisnickoIme == Vozac.KorisnickoIme).FirstOrDefault();
                    var KompanijaUsername = Context.Kompanija!.Where( p => p.KorisnickoIme == Vozac.KorisnickoIme).FirstOrDefault();
                    var DispecerUsername = Context.Dispecer!.Where(p=>p.KorisnickoIme == Vozac.KorisnickoIme).FirstOrDefault();
                    if(KompanijaUsername!= null || VozacUsername!= null || DispecerUsername!= null)
                        return BadRequest("Vec postoji nalog sa tim korisnickim imenom");
                }
                Voz.Ime = Vozac.Ime;
                Voz.Prezime = Vozac.Prezime;
                Voz.JMBG = Vozac.JMBG;
                Voz.Email = Vozac.Email;
                Voz.KorisnickoIme = Vozac.KorisnickoIme;
                Voz.Slika=Vozac.Slika;
                Voz.BrojTelefona=Vozac.BrojTelefona;
                Context.Vozac.Update(Voz);
                await Context.SaveChangesAsync();
                return Ok($"Vozac izmenjen");
        }
        else
        {
            return BadRequest("Vozac nije pronadjena u bazi");
        }
      }
      catch(Exception ex){
         return BadRequest(ex.Message);
      }
  }
  [Authorize(Roles ="Dispecer")]
  [Route("DeleteVozac/{id}")]
  [HttpDelete]
  public async Task<ActionResult> DeleteVozac(int id)
  {
    try{
             
             var Vozac = Context.Vozac!.Find(id);
             if(Vozac!=null)
             {
                var vozila=await Context.Vozilo!.Where(p=>p.Vozac==Vozac).ToListAsync();
                var prikolice=await Context.Prikolica!.Where(p=>p.Vozac==Vozac).ToListAsync();
                var ponudjene=await Context.PonudjenaTura!.Where(p=>p.Vozac==Vozac).ToListAsync();
                var prihvacena=await Context.PrihvacenaTura!.Where(p=>p.Vozac==Vozac).ToListAsync();
                var dodeljena= await Context.DodeljeneTure!.Where(p=>p.Vozac==Vozac).ToListAsync();
                var ocene=await Context.Ocena!.Where(p=>p.Vozac==Vozac).ToListAsync();
                var favorizacije=await Context.Favorizacija!.Where(p=>p.Vozac==Vozac).ToListAsync();

                foreach(var f in favorizacije)
                {
                    Context.Remove(f);
                }
                await Context.SaveChangesAsync();

                 foreach(var f in ocene)
                {
                    Context.Remove(f);
                }
                await Context.SaveChangesAsync();


                 foreach(var f in dodeljena)
                {
                    Context.Remove(f);
                }
                await Context.SaveChangesAsync();

                 foreach(var f in prihvacena)
                {
                    Context.Remove(f);
                }
                await Context.SaveChangesAsync();

                 foreach(var f in ponudjene)
                {
                    Context.Remove(f);
                }
                await Context.SaveChangesAsync();

                 foreach(var f in prikolice)
                {
                    Context.Remove(f);
                }
                await Context.SaveChangesAsync();

                 foreach(var f in vozila)
                {
                    Context.Remove(f);
                }
                await Context.SaveChangesAsync();



                Context.Vozac.Remove(Vozac);
                await Context.SaveChangesAsync();
                return Ok($"Vozac obrisan");

             }
            else
            {
                return BadRequest("Vozac nije pronadjen u bazi");
            }
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
   }
   [Authorize(Roles ="Vozac,Dispecer")]
   [Route("UpdateSifra/{id}/{staraSifra}/{novaSifra}")]
   [HttpPut]
  public async Task<IActionResult>UpdateSifra(int id,string staraSifra,string novaSifra)
    {
        try{
            var Vozac = Context.Vozac!.Find(id);
             
            if(Vozac!=null)
            {
                string sifra;
                
                if(BCrypt.Net.BCrypt.Verify(staraSifra,Vozac.Sifra))
                {
                    sifra = novaSifra;
                }
                else{
                    return BadRequest("Pogresna stara šifra");
                }

                if(sifra.Length<8 || sifra.Length>20 || Regex.IsMatch(sifra,"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#!@$^&*-]).{8,}$")==false)
                    return BadRequest("Pogresan format nove sifre (sifra mora da ima jedno veliko,jedno malo slovo, jedan specijalni znak i najmanja duzina je 8 karaktera)");
                
                string cryptNovaSifra = BCrypt.Net.BCrypt.HashPassword(sifra,10);
                
                Vozac.Sifra=cryptNovaSifra;
                Context.Vozac.Update(Vozac);
                await Context.SaveChangesAsync();
                return Ok($"Sifra uspesno izmenjena");
            }
            else
            {
                return BadRequest("Vozac nije pronadjena u bazi");
            }
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles ="Kompanija,Dispecer")]
    [Route("GetVozace")]
    [HttpGet]
    public async Task<IActionResult>GetVozace()
    {
        try{
            var Vozaci = await Context.Vozac!.ToListAsync();
            return Ok(Vozaci);
        }
        catch(Exception ex)
        {
           return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [Route("GetVozaca/{id}")]
    [HttpGet]
    public async Task<IActionResult>GetVozace(int id)
    {
        try{
            var Vozac = await Context.Vozac!.FindAsync(id);
            return Ok(Vozac);
        }
        catch(Exception ex)
        {
           return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles ="Vozac")]
    [Route("GetOcene/{id}")]
    [HttpGet]
    public async Task<IActionResult>GetOcene(int id)
    {
        try{
            var Ocene = await Context.Ocena!.Include(p=>p.Kompanija).Include(p=>p.Vozac).Where(p=> p!.Vozac!.ID==id).ToListAsync();
            return Ok(Ocene);
        }
        catch(Exception ex)
        {
           return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [Route("GetSrednjuOcenu/{id}")]
    [HttpGet]
    public async Task<IActionResult>GetSrednjuOcenu(int id)
    {
        try{
            var Ocene = await Context.Ocena!.Where(p=> p!.Vozac!.ID==id).ToListAsync();
            var ukupniBrojOcena = Ocene.Count;
            var ukupneOcene = Ocene.Sum(item => item.Broj);
            decimal srednja = 0;
            if(ukupniBrojOcena != 0)
            {
                srednja = (decimal)ukupneOcene/ukupniBrojOcena;
            }
            var brojJedinicafunkcija = await Context.Ocena!.Where(p=> p!.Vozac!.ID==id && p!.Broj==1).ToListAsync();
            var brojDvojkefunkcija = await Context.Ocena!.Where(p=> p!.Vozac!.ID==id && p!.Broj==2).ToListAsync();
            var brojTrojkefunkcija = await Context.Ocena!.Where(p=> p!.Vozac!.ID==id && p!.Broj==3).ToListAsync();
            var brojCetvorkefunkcija = await Context.Ocena!.Where(p=> p!.Vozac!.ID==id && p!.Broj==4).ToListAsync();
            var brojPeticefunkcija = await Context.Ocena!.Where(p=> p!.Vozac!.ID==id && p!.Broj==5).ToListAsync();
            decimal brojJedinica = Math.Floor(ukupniBrojOcena == 0? 0: (decimal)brojJedinicafunkcija.Count*100/ukupniBrojOcena);
            decimal brojDvojki = Math.Floor(ukupniBrojOcena == 0? 0: (decimal)brojDvojkefunkcija.Count*100/ukupniBrojOcena);
            decimal brojTrojki = Math.Floor(ukupniBrojOcena == 0? 0: (decimal)brojTrojkefunkcija.Count*100/ukupniBrojOcena);
            decimal brojCetvorke = Math.Floor(ukupniBrojOcena == 0? 0: (decimal)brojCetvorkefunkcija.Count*100/ukupniBrojOcena);
            decimal brojPetica =Math.Floor( ukupniBrojOcena == 0? 0: (decimal)brojPeticefunkcija.Count*100/ukupniBrojOcena);
            return Ok(new {
                ukupniBrojOcena,
                srednja,
                brojJedinica,
                brojDvojki,
                brojTrojki,
                brojCetvorke,
                brojPetica
            });
        }
        catch(Exception ex)
        {
           return BadRequest(ex.Message);
        }
    }
    
}