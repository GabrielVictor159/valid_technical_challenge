import { useState, useEffect, useCallback } from "react";
import NavLayout from "@/components/NavLayout/NavLayout";
import PageHeader from "@/components/PageHeader/PageHeader";
import { useParams } from "react-router-dom";
import orderService from "@/services/orderService";
import type { SearchOrderResponse } from "@/types/order/SearchOrderResponse";

export default function OrderDetails() {
  const { id } = useParams<{ id: string }>();
  
  const [order, setOrder] = useState<SearchOrderResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(5);

  const fetchOrder = useCallback(async () => {
    if (!id) return;
    try {
      const data = await orderService.getById(Number(id));
      setOrder(data);
    } catch (error) {
      console.error("Erro ao carregar detalhes do pedido:", error);
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    fetchOrder();

    const interval = setInterval(() => {
      fetchOrder(); 
    }, 5000);

    return () => clearInterval(interval); 
  }, [fetchOrder]);

  if (loading && !order) {
    return (
      <NavLayout>
        <div className="container-fluid py-4 d-flex justify-content-center">
          <div className="spinner-border text-primary" role="status"></div>
        </div>
      </NavLayout>
    );
  }

  if (!order) {
    return (
      <NavLayout>
        <div className="container-fluid py-4 text-center">
          <h3>Pedido não encontrado</h3>
        </div>
      </NavLayout>
    );
  }

  const indexOfLast = currentPage * itemsPerPage;
  const indexOfFirst = indexOfLast - itemsPerPage;
  const currentOperations = order.operations.slice(indexOfFirst, indexOfLast);
  const totalPages = Math.ceil(order.operations.length / itemsPerPage);

  const getStatusBadge = (status: number) => {
    const statusMap: Record<number, { label: string; class: string }> = {
      1: { label: "Recebido", class: "bg-info" },
      2: { label: "Em Processamento", class: "bg-primary" },
      3: { label: "Processado", class: "bg-success" },
      4: { label: "Erro", class: "bg-danger" },
    };
    const current = statusMap[status] || { label: "Desconhecido", class: "bg-secondary" };
    return <span className={`badge ${current.class}`}>{current.label}</span>;
  };

  return (
    <NavLayout>
      <div className="container-fluid py-4" style={{ minHeight: "90vh" }}>
        <PageHeader title={`Detalhes do Pedido #${order.numberOrder}`} />

        <div className="card shadow-sm p-4 mb-4 border-0">
          <div className="d-flex justify-content-between align-items-center mb-3">
             <h5 className="mb-0">Informações Gerais</h5>
             <small className="text-muted">ID Interno: {order.id}</small>
          </div>

          <table className="table table-bordered align-middle">
            <tbody>
              <tr>
                <th className="bg-light" style={{ width: 250 }}>Número do Pedido</th>
                <td>{order.numberOrder}</td>
              </tr>
              <tr>
                <th className="bg-light">Total</th>
                <td className="fw-bold text-primary">
                  {order.totalPrice.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                </td>
              </tr>
              <tr>
                <th className="bg-light">Status Atual</th>
                <td>{getStatusBadge(order.status)}</td>
              </tr>
              <tr>
                <th className="bg-light">Data de Criação</th>
                <td>{new Date(order.createdDate).toLocaleString('pt-BR')}</td>
              </tr>
            </tbody>
          </table>
        </div>

        <div className="card shadow-sm p-4 border-0">
          <div className="d-flex justify-content-between align-items-center mb-3">
            <h5 className="mb-0">Histórico de Operações</h5>
            <div className="d-flex align-items-center gap-2">
              <small className="text-muted">Itens por página:</small>
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
                  <th style={{ width: "20%" }}>Status</th>
                  <th>Mensagem</th>
                  <th style={{ width: "25%" }}>Data</th>
                </tr>
              </thead>
              <tbody>
                {currentOperations.map((op, index) => (
                  <tr key={index}>
                    <td>{getStatusBadge(op.status)}</td>
                    <td>{op.message}</td>
                    <td>{new Date(op.createdDate).toLocaleString('pt-BR')}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {/* Paginação */}
          {totalPages > 1 && (
            <div className="d-flex justify-content-center align-items-center gap-4 mt-3">
              <span
                role="button"
                className={`fs-5 ${currentPage === 1 ? "text-muted" : "text-primary"}`}
                onClick={() => currentPage > 1 && setCurrentPage(currentPage - 1)}
                style={{ cursor: currentPage === 1 ? "not-allowed" : "pointer", userSelect: "none" }}
              >
                ‹
              </span>

              <div className="d-flex gap-3">
                {Array.from({ length: totalPages }, (_, i) => (
                  <span
                    key={i}
                    role="button"
                    onClick={() => setCurrentPage(i + 1)}
                    className={`fw-semibold px-2 py-1 ${currentPage === i + 1 ? "text-primary border-bottom border-primary" : "text-secondary"}`}
                    style={{ cursor: "pointer", userSelect: "none" }}
                  >
                    {i + 1}
                  </span>
                ))}
              </div>

              <span
                role="button"
                className={`fs-5 ${currentPage === totalPages ? "text-muted" : "text-primary"}`}
                onClick={() => currentPage < totalPages && setCurrentPage(currentPage + 1)}
                style={{ cursor: currentPage === totalPages ? "not-allowed" : "pointer", userSelect: "none" }}
              >
                ›
              </span>
            </div>
          )}
        </div>
      </div>
    </NavLayout>
  );
}