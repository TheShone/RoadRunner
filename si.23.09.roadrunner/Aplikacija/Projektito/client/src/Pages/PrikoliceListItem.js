import React, { useState } from "react";
import { Image } from "react-native";
import { Modal } from "react-bootstrap";

const PrikolicaListItem = ({ prikolica, handleDelete, handleUpdate }) => {
  const [showAlert, setShowAlert] = useState(false);
  const handleClose = () => setShowAlert(false);
  const handleObrisi = () => {
    handleDelete(prikolica.id);
    setShowAlert(false);
  };
  return (
    <>
      <Modal
        show={showAlert}
        onHide={handleClose}
        backdrop="static"
        keyboard={false}
      >
        <Modal.Header closeButton>
          <Modal.Title>Brisanje Prikolice</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          Da li ste sigurni da želite da obrišete prikolicu?
        </Modal.Body>
        <Modal.Footer>
          <button className="btn-prim" onClick={handleClose}>
            Nazad
          </button>
          <button className="btn-danger" onClick={handleObrisi}>
            Obrisi
          </button>
        </Modal.Footer>
      </Modal>
      <tr className="hover:bg-gray-50 h-40">
        <td className=" h-full w-1/5 px-14 font-normal text-gray-900 ">
          <Image
            style={{
              height: 100,
              width: 100,
              aspectRatio: 2.5,
              borderRadius: 200 / 2,
            }}
            source={{ uri: prikolica.slika }}
          />
        </td>
        <td className="px-6 py-4">
          <div className="text-sm">
            <div className="">
              Zapremina:
              {prikolica.zapremina === null ? "-" : prikolica.zapremina}
            </div>
            <div className="">
              Nosivost: {prikolica.nosivost === null ? "-" : prikolica.nosivost}
            </div>
          </div>
        </td>
        <td className="px-6 py-4">
          <div className="">
            Dužina: {prikolica.duzina === null ? "-" : prikolica.duzina}
          </div>
          <div className="">
            Širina: {prikolica.sirina === null ? "-" : prikolica.sirina}
          </div>
          <div className="">
            Visina: {prikolica.visina === null ? "-" : prikolica.visina}
          </div>
        </td>

        <td className="px-6 py-4">
          <div>Tablice: {prikolica.tablice}</div>{" "}
          <div>Tip: {prikolica.tipPrikolice.tip}</div>
        </td>

        <td className="px-6 py-4">
          <div className="flex justify-end gap-4">
            <button
              id={prikolica.id}
              className="text-green-400"
              onClick={(e) => {
                handleUpdate(e);
              }}
              x-data="{ tooltip: 'Edite' }"
              href="#"
            >
              <svg
                id={prikolica.id}
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth="1.5"
                stroke="currentColor"
                className="h-6 w-6"
                x-tooltip="tooltip"
              >
                <path
                  id={prikolica.id}
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L6.832 19.82a4.5 4.5 0 01-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 011.13-1.897L16.863 4.487zm0 0L19.5 7.125"
                />
              </svg>
            </button>
            <button
              type="submit"
              x-data="{ tooltip: 'Delete' }"
              className="text-red-600"
              href="#"
              onClick={() => {
                setShowAlert(true);
              }}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth="1.5"
                stroke="currentColor"
                className="h-6 w-6"
                x-tooltip="tooltip"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0"
                />
              </svg>
            </button>
          </div>
        </td>
      </tr>
    </>
  );
};

export default PrikolicaListItem;
