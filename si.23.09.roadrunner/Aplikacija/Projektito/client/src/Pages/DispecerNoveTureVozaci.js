import React from "react";
import { UserContext } from "../UserContext";
import { useState, useContext } from "react";
import Cookies from "js-cookie";
import axios from "axios";
import { useEffect } from "react";
import DispecerNoveTureVozaciListItem from "./DispecerNoveTureVozaciListItem";
import { Modal } from "react-bootstrap";
import LoadingPage from "./LoadingPage";

const DispecerNoveTureVozaci = ({
  turaId,
  poslati,
  setPoslati,
  setVozaci,
  setMapa,
}) => {
  const { user, setUser } = useContext(UserContext);
  const config = {
    headers: { Authorization: `Bearer ${Cookies.get("Token")}` },
  };
  const [currentItems, setCurrentItems] = useState([]);
  const [ready, setReady] = useState(false);
  const [order, setOrder] = useState("ASC");
  const [stanje, setStanje] = useState(0);
  const [odabraneStavke, setOdabraneStavke] = useState([]);

  const [stringGreska, setStringGreska] = useState("");
  const [showAlert, setShowAlert] = useState(false);

  const toggleOdabranaStavka = (stavka) => {
    const novaLista = odabraneStavke.includes(stavka)
      ? odabraneStavke.filter((item) => item !== stavka)
      : [...odabraneStavke, stavka];
    setOdabraneStavke(novaLista);
  };

  useEffect(() => {
    if (user) {
      axios
        .get(`/Dispecer/IzlistajVozaceZaTuru/${turaId}`, config)
        .then((response) => {
          setCurrentItems(response.data);
          setReady(true);
        })
        .catch((err) => {
          console.log("Error:" + err.message);
        });
    }
  }, [ready, user, poslati, turaId]);
  const sorting = (col) => {
    if (order === "ASC") {
      const sorted = [...currentItems].sort((a, b) =>
        a[col] > b[col] ? 1 : -1
      );
      setCurrentItems(sorted);
      setOrder("DSC");
    }
    if (order === "DSC") {
      const sorted = [...currentItems].sort((a, b) =>
        a[col] < b[col] ? 1 : -1
      );
      setCurrentItems(sorted);
      setOrder("ASC");
    }
  };
  const handlePrihvati = () => {
    if (odabraneStavke != "") {
      axios
        .post(
          `Tura/AddPonudjenaTura/${turaId}/${user.id}/${odabraneStavke}`,
          {},
          config
        )
        .then((response) => {
          setPoslati(!poslati);
          setMapa(false);
          setVozaci("");
        })
        .catch((err) => {
          setStringGreska(`Error: + ${err.message}`);
          setShowAlert(true);
        });
    } else {
      setStringGreska("Niste izabrali vozaca.");
      setShowAlert(true);
    }
  };

  const handleClose = () => setShowAlert(false);

  if (!ready) {
    return <LoadingPage />;
  } else {
    return (
      <>
        <Modal
          show={showAlert}
          onHide={handleClose}
          backdrop="static"
          keyboard={false}
        >
          <Modal.Header closeButton>
            <Modal.Title>Greška!</Modal.Title>
          </Modal.Header>
          <Modal.Body>{stringGreska}</Modal.Body>
          <Modal.Footer>
            <button className="btn-prim" onClick={handleClose}>
              Zatvori
            </button>
          </Modal.Footer>
        </Modal>

        <div className="flex flex-col mt-8">
          <div className="overflow-auto sm:rounded-lg">
            <table className="w-full text-sm text-left text-gray-500  shadow-md ">
              <thead className="text-xs text-white uppercase bg-primary">
                <tr>
                  <th scope="col" className="px-6 py-3 whitespace-nowrap just">
                    <div className="flex flex-row justify-center">Profilna slika</div>
                  </th>
                  <th scope="col" className="px-6 py-3 whitespace-nowrap ">
                  <div className="flex flex-row justify-center">
                    <button
                      className="flex flex-row uppercase"
                      onClick={() => sorting("ime")}
                    >
                      Ime
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="w-3 h-3 ml-1 mb-0"
                        aria-hidden="true"
                        fill="currentColor"
                        viewBox="0 0 320 512"
                      >
                        <path d="M27.66 224h264.7c24.6 0 36.89-29.78 19.54-47.12l-132.3-136.8c-5.406-5.406-12.47-8.107-19.53-8.107c-7.055 0-14.09 2.701-19.45 8.107L8.119 176.9C-9.229 194.2 3.055 224 27.66 224zM292.3 288H27.66c-24.6 0-36.89 29.77-19.54 47.12l132.5 136.8C145.9 477.3 152.1 480 160 480c7.053 0 14.12-2.703 19.53-8.109l132.3-136.8C329.2 317.8 316.9 288 292.3 288z" />
                      </svg>
                    </button>
                    </div>
                  </th>
                  <th scope="col" className="px-6 py-3 whitespace-nowrap ">
                  <div className="flex flex-row justify-center">
                    <button
                      className="flex flex-row uppercase"
                      onClick={() => sorting("prezime")}
                    >
                      Prezime
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="w-3 h-3 ml-1 mb-0"
                        aria-hidden="true"
                        fill="currentColor"
                        viewBox="0 0 320 512"
                      >
                        <path d="M27.66 224h264.7c24.6 0 36.89-29.78 19.54-47.12l-132.3-136.8c-5.406-5.406-12.47-8.107-19.53-8.107c-7.055 0-14.09 2.701-19.45 8.107L8.119 176.9C-9.229 194.2 3.055 224 27.66 224zM292.3 288H27.66c-24.6 0-36.89 29.77-19.54 47.12l132.5 136.8C145.9 477.3 152.1 480 160 480c7.053 0 14.12-2.703 19.53-8.109l132.3-136.8C329.2 317.8 316.9 288 292.3 288z" />
                      </svg>
                    </button>
                    </div>
                  </th>
                  <th scope="col" className="px-6 py-3 whitespace-nowrap ">
                  <div className="flex flex-row justify-center">
                    <button
                      className="flex flex-row uppercase"
                      onClick={() => sorting("jmbg")}
                    >
                      JMBG
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="w-3 h-3 ml-1 mb-0"
                        aria-hidden="true"
                        fill="currentColor"
                        viewBox="0 0 320 512"
                      >
                        <path d="M27.66 224h264.7c24.6 0 36.89-29.78 19.54-47.12l-132.3-136.8c-5.406-5.406-12.47-8.107-19.53-8.107c-7.055 0-14.09 2.701-19.45 8.107L8.119 176.9C-9.229 194.2 3.055 224 27.66 224zM292.3 288H27.66c-24.6 0-36.89 29.77-19.54 47.12l132.5 136.8C145.9 477.3 152.1 480 160 480c7.053 0 14.12-2.703 19.53-8.109l132.3-136.8C329.2 317.8 316.9 288 292.3 288z" />
                      </svg>
                    </button>
                    </div>
                  </th>
                  <th scope="col" className="px-6 py-3 whitespace-nowrap ">
                  <div className="flex flex-row justify-center">
                    <button
                      className="flex flex-row uppercase"
                      onClick={() => sorting("email")}
                    >
                      Email
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="w-3 h-3 ml-1 mb-0"
                        aria-hidden="true"
                        fill="currentColor"
                        viewBox="0 0 320 512"
                      >
                        <path d="M27.66 224h264.7c24.6 0 36.89-29.78 19.54-47.12l-132.3-136.8c-5.406-5.406-12.47-8.107-19.53-8.107c-7.055 0-14.09 2.701-19.45 8.107L8.119 176.9C-9.229 194.2 3.055 224 27.66 224zM292.3 288H27.66c-24.6 0-36.89 29.77-19.54 47.12l132.5 136.8C145.9 477.3 152.1 480 160 480c7.053 0 14.12-2.703 19.53-8.109l132.3-136.8C329.2 317.8 316.9 288 292.3 288z" />
                      </svg>
                    </button>
                    </div>
                  </th>
                  <th scope="col" className="px-6 py-3 whitespace-nowrap ">
                  <div className="flex flex-row justify-center">
                    <button
                      className="flex flex-row uppercase"
                      onClick={() => sorting("korisnickoIme")}
                    >
                      Korisničko ime
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="w-3 h-3 ml-1 mb-0"
                        aria-hidden="true"
                        fill="currentColor"
                        viewBox="0 0 320 512"
                      >
                        <path d="M27.66 224h264.7c24.6 0 36.89-29.78 19.54-47.12l-132.3-136.8c-5.406-5.406-12.47-8.107-19.53-8.107c-7.055 0-14.09 2.701-19.45 8.107L8.119 176.9C-9.229 194.2 3.055 224 27.66 224zM292.3 288H27.66c-24.6 0-36.89 29.77-19.54 47.12l132.5 136.8C145.9 477.3 152.1 480 160 480c7.053 0 14.12-2.703 19.53-8.109l132.3-136.8C329.2 317.8 316.9 288 292.3 288z" />
                      </svg>
                    </button>
                    </div>
                  </th>
                  <th scope="col" className="px-6 py-3 whitespace-nowrap">
                  <div className="flex flex-row justify-center">
                    <button
                      className="flex flex-row uppercase"
                      onClick={() => sorting("brojTelefona")}
                    >
                      Broj telefona
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        className="w-3 h-3 ml-1 mb-0"
                        aria-hidden="true"
                        fill="currentColor"
                        viewBox="0 0 320 512"
                      >
                        <path d="M27.66 224h264.7c24.6 0 36.89-29.78 19.54-47.12l-132.3-136.8c-5.406-5.406-12.47-8.107-19.53-8.107c-7.055 0-14.09 2.701-19.45 8.107L8.119 176.9C-9.229 194.2 3.055 224 27.66 224zM292.3 288H27.66c-24.6 0-36.89 29.77-19.54 47.12l132.5 136.8C145.9 477.3 152.1 480 160 480c7.053 0 14.12-2.703 19.53-8.109l132.3-136.8C329.2 317.8 316.9 288 292.3 288z" />
                      </svg>
                    </button>
                    </div>
                  </th>
                  <th scope="col" className="px-6 py-3 whitespace-nowrap">
                    <div className="flex flex-row justify-center">Srednja ocena</div>
                  </th>
                  <th scope="col" className="px-6 py-3 whitespace-nowrap">
                    <div className="flex flex-row justify-center">Ponudi</div>
                  </th>
                </tr>
              </thead>

              <tbody className="text-center">
                {currentItems.map((item) => (
                  <DispecerNoveTureVozaciListItem
                    item={item}
                    key={item.id}
                    checked={odabraneStavke.includes(item.id)}
                    toggleOdabranaStavka={toggleOdabranaStavka}
                  />
                ))}
                {currentItems.length === 0 && (
                  <tr>
                    <th colSpan="10" className="text-center">
                      Nepostoje adekvatni vozaci
                    </th>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
          {currentItems.length !== 0 && (
            <div className="flex justify-center mt-5">
              <button className="btn-prim" onClick={handlePrihvati}>
                Ponudi Turu
              </button>
            </div>
          )}
        </div>
      </>
    );
  }
};

export default DispecerNoveTureVozaci;
