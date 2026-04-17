import { useState, useEffect, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import Table from "react-bootstrap/Table";
import Spinner from "react-bootstrap/Spinner";
import orderService from "@/services/orderService";
import type { SearchOrderResponse } from "@/types/order/SearchOrderResponse";
import "./OrdersTable.css"

export default function OrdersTable() {
  const navigate = useNavigate();

  const [orders, setOrders] = useState<SearchOrderResponse[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(true);

  const [search, setSearch] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);

  const fetchOrders = useCallback(async () => {
    setLoading(true);
    try {
      const response = await orderService.getPaged({
        page: currentPage,
        pageSize: itemsPerPage,
        searchTerm: search || undefined,
      });

      setOrders(response.items);
      setTotalCount(response.totalCount);
    } catch (error) {
      console.error("Erro ao carregar pedidos:", error);
    } finally {
      setLoading(false);
    }
  }, [currentPage, itemsPerPage, search]);

  useEffect(() => {
    fetchOrders();
  }, [fetchOrders]);

  const totalPages = Math.ceil(totalCount / itemsPerPage);

  return (
    <div className="card shadow-sm border-0 p-3">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h4 className="mb-0 fw-bold text-secondary">Pedidos</h4>

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

      <div className="input-group mb-3">
        <input
          type="text"
          className="form-control"
          placeholder="Buscar por número ou cliente..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && fetchOrders()}
        />
        <button
          className="btn btn-primary"
          type="button"
          onClick={() => {
            setCurrentPage(1);
            fetchOrders();
          }}
        >
          Buscar
        </button>
      </div>

      <div className="table-responsive">
        <Table hover striped className="align-middle mb-0">
          <thead className="table-light">
            <tr>
              <th>Nº Pedido</th>
              <th>Total</th>
              <th>Data</th>
              <th className="text-center">Ações</th>
            </tr>
          </thead>

          <tbody>
            {loading ? (
              <tr>
                <td colSpan={5} className="text-center py-5">
                  <Spinner animation="border" variant="primary" size="sm" className="me-2" />
                  Carregando pedidos...
                </td>
              </tr>
            ) : orders.length > 0 ? (
              orders.map((order) => (
                <tr key={order.id}>
                  <td className="fw-bold">{order.numberOrder}</td>
                  <td>
                    {order.totalPrice.toLocaleString("pt-BR", {
                      style: "currency",
                      currency: "BRL",
                    })}
                  </td>
                  <td>{new Date(order.createdDate).toLocaleString("pt-BR")}</td>
                  <td className="text-center">
                    <button
                      className="btn btn-sm btn-outline-primary shadow-sm"
                      onClick={() => navigate(`/order-details/${order.id}`)}
                      title="Ver detalhes"
                    >
                      <img src="/search.svg" className="icon-search"/>
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={5} className="text-center py-4 text-muted">
                  Nenhum pedido encontrado.
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      </div>

      <div className="d-flex justify-content-between align-items-center mt-3">
        <small className="text-muted">
          Mostrando {orders.length > 0 ? (currentPage - 1) * itemsPerPage + 1 : 0} -{" "}
          {Math.min(currentPage * itemsPerPage, totalCount)} de {totalCount}
        </small>

        {totalPages > 1 && (
          <div className="d-flex align-items-center gap-4">
            <span
              role="button"
              className={`fs-4 ${currentPage === 1 ? "text-muted" : "text-primary"}`}
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
                    fontSize: "1rem",
                    userSelect: "none",
                  }}
                >
                  {i + 1}
                </span>
              ))}
            </div>

            <span
              role="button"
              className={`fs-4 ${currentPage === totalPages ? "text-muted" : "text-primary"}`}
              onClick={() => currentPage < totalPages && setCurrentPage(currentPage + 1)}
              style={{
                cursor: currentPage === totalPages ? "not-allowed" : "pointer",
                userSelect: "none",
              }}
            >
              ›
            </span>
          </div>
        )}
      </div>
    </div>
  );
}