import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Table from "react-bootstrap/Table";

interface Order {
  id: number;
  orderNumber: string;
  cliente: string;
  total: number;
  createdAt: string;
}

const mockOrders: Order[] = [
  {
    id: 1,
    orderNumber: "1231",
    cliente: "João",
    total: 120,
    createdAt: "2026-04-16 10:30",
  },
  {
    id: 2,
    orderNumber: "421",
    cliente: "Maria",
    total: 250,
    createdAt: "2026-04-16 11:10",
  },
  {
    id: 3,
    orderNumber: "789",
    cliente: "Carlos",
    total: 80,
    createdAt: "2026-04-16 12:05",
  },
];

export default function OrdersTable() {
  const [search, setSearch] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);

  const navigate = useNavigate();

  const filtered = mockOrders.filter((o) =>
    o.cliente.toLowerCase().includes(search.toLowerCase()),
  );

  const indexOfLast = currentPage * itemsPerPage;
  const indexOfFirst = indexOfLast - itemsPerPage;
  const currentItems = filtered.slice(indexOfFirst, indexOfLast);

  const totalPages = Math.ceil(filtered.length / itemsPerPage);

  return (
    <div className="card p-3">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h4 className="mb-0">Pedidos</h4>

        <div className="d-flex align-items-center gap-2">
          <small className="text-muted">Items:</small>

          <select
            className="form-select form-select-sm"
            style={{ width: "80px" }}
            value={itemsPerPage}
            onChange={(e) => {
              setItemsPerPage(Number(e.target.value));
              setCurrentPage(1);
            }}
          >
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={20}>20</option>
          </select>
        </div>
      </div>

      <div className="input-group mb-3">
        <input
            type="text"
            className="form-control"
            placeholder="Buscar cliente..."
            value={search}
            onChange={(e) => {
            setSearch(e.target.value);
            setCurrentPage(1);
            }}
        />

        <button
            className="btn btn-outline-primary"
            type="button"
            onClick={() => setCurrentPage(1)}
        >
             Buscar
        </button>
        </div>

      <Table hover striped className="align-middle">
        <thead className="table-light">
          <tr>
            <th>Nº Pedido</th>
            <th>Cliente</th>
            <th>Total</th>
            <th>Data</th>
            <th>Ações</th>
          </tr>
        </thead>

        <tbody>
          {currentItems.map((order) => (
            <tr key={order.id}>
              <td>{order.orderNumber}</td>
              <td>{order.cliente}</td>
              <td>R$ {order.total}</td>
              <td>{order.createdAt}</td>
              <td>
                <button
                  className="btn btn-sm btn-outline-primary"
                  onClick={() => navigate(`/order-details/${order.id}`)}
                >
                  🔍
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <div className="d-flex justify-content-between align-items-center mt-3">
        <small className="text-muted">
          Mostrando {indexOfFirst + 1} -{" "}
          {Math.min(indexOfLast, filtered.length)} de {filtered.length}
        </small>

        <div className="d-flex align-items-center gap-4">
          <span
            role="button"
            className={`fs-4 ${
              currentPage === 1 ? "text-muted" : "text-primary"
            }`}
            onClick={() => currentPage > 1 && setCurrentPage(currentPage - 1)}
            style={{
              cursor: currentPage === 1 ? "not-allowed" : "pointer",
              userSelect: "none",
            }}
          >
            ‹
          </span>

          <div className="d-flex gap-3">
            {Array.from({ length: totalPages }, (_, i) => (
              <span
                key={i}
                role="button"
                onClick={() => setCurrentPage(i + 1)}
                className={`fw-semibold px-2 py-1 ${
                  currentPage === i + 1
                    ? "text-primary border-bottom border-primary"
                    : "text-secondary"
                }`}
                style={{
                  cursor: "pointer",
                  fontSize: "1.05rem",
                  userSelect: "none",
                }}
              >
                {i + 1}
              </span>
            ))}
          </div>

          <span
            role="button"
            className={`fs-4 ${
              currentPage === totalPages ? "text-muted" : "text-primary"
            }`}
            onClick={() =>
              currentPage < totalPages && setCurrentPage(currentPage + 1)
            }
            style={{
              cursor: currentPage === totalPages ? "not-allowed" : "pointer",
              userSelect: "none",
            }}
          >
            ›
          </span>
        </div>
      </div>
    </div>
  );
}
