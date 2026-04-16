import { useState } from "react";
import NavLayout from "@/components/NavLayout";
import PageHeader from "@/components/PageHeader";
import { useParams } from "react-router-dom";

type OrderStatusEnum = "Pending" | "Processing" | "Completed" | "Cancelled";

interface Operation {
  status: OrderStatusEnum;
  message: string;
  createdDate: string;
}

const mockOperations: Operation[] = [
  { status: "Pending", message: "Pedido criado", createdDate: "16/04/2026 10:30" },
  { status: "Processing", message: "Pagamento confirmado", createdDate: "16/04/2026 10:45" },
  { status: "Completed", message: "Pedido finalizado", createdDate: "16/04/2026 11:10" },
];

export default function OrderDetails() {
  const { id } = useParams();

  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(5);

  const indexOfLast = currentPage * itemsPerPage;
  const indexOfFirst = indexOfLast - itemsPerPage;

  const currentOperations = mockOperations.slice(
    indexOfFirst,
    indexOfLast
  );

  const totalPages = Math.ceil(mockOperations.length / itemsPerPage);

  return (
    <NavLayout>
      <div className="container-fluid py-4" style={{ minHeight: "90vh" }}>
        <PageHeader title="Detalhes do Pedido" />

        <div className="card p-4 mb-4">
          <h5 className="mb-3">Pedido #{id}</h5>

          <table className="table table-bordered align-middle">
            <tbody>
              <tr>
                <th className="bg-light" style={{ width: 200 }}>
                  Número do Pedido
                </th>
                <td>123</td>
              </tr>

              <tr>
                <th className="bg-light">Cliente</th>
                <td>Exemplo</td>
              </tr>

              <tr>
                <th className="bg-light">Total</th>
                <td>R$ 100</td>
              </tr>

              <tr>
                <th className="bg-light">Status</th>
                <td>
                  <span className="badge bg-primary">Em andamento</span>
                </td>
              </tr>

              <tr>
                <th className="bg-light">Data de Criação</th>
                <td>16/04/2026 10:30</td>
              </tr>
            </tbody>
          </table>
        </div>

        <div className="card p-4">

          <div className="d-flex justify-content-between align-items-center mb-3">

            <h5 className="mb-0">Histórico de Operações</h5>

            <div className="d-flex align-items-center gap-2">
              <small className="text-muted">Itens:</small>

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

          <div className="table-responsive">
            <table className="table table-striped table-hover align-middle">
              <thead className="table-light">
                <tr>
                  <th>Status</th>
                  <th>Mensagem</th>
                  <th>Data</th>
                </tr>
              </thead>

              <tbody>
                {currentOperations.map((op, index) => (
                  <tr key={index}>
                    <td>
                      <span className="badge bg-secondary">
                        {op.status}
                      </span>
                    </td>
                    <td>{op.message}</td>
                    <td>{op.createdDate}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className="d-flex justify-content-center align-items-center gap-4 mt-3">

            <span
              role="button"
              className={`fs-5 ${
                currentPage === 1 ? "text-muted" : "text-primary"
              }`}
              onClick={() =>
                currentPage > 1 && setCurrentPage(currentPage - 1)
              }
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
                  style={{ cursor: "pointer", userSelect: "none" }}
                >
                  {i + 1}
                </span>
              ))}
            </div>

            {/* próximo */}
            <span
              role="button"
              className={`fs-5 ${
                currentPage === totalPages
                  ? "text-muted"
                  : "text-primary"
              }`}
              onClick={() =>
                currentPage < totalPages &&
                setCurrentPage(currentPage + 1)
              }
              style={{
                cursor:
                  currentPage === totalPages
                    ? "not-allowed"
                    : "pointer",
                userSelect: "none",
              }}
            >
              ›
            </span>

          </div>
        </div>
      </div>
    </NavLayout>
  );
}