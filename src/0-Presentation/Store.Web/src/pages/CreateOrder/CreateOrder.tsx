import { useState } from "react";
import NavLayout from "@/components/NavLayout/NavLayout";
import PageHeader from "@/components/PageHeader/PageHeader";
import orderService from "@/services/orderService";
import { toast } from "react-toastify";

export default function CreateOrder() {
  const [numero, setNumero] = useState("");
  const [cliente, setCliente] = useState("");
  const [valor, setValor] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      const request = {
        numberOrder: numero,
        client: cliente,
        totalPrice: Number(valor),
      };

      await orderService.create(request);

      toast.success(`Pedido ${numero} criado com sucesso!`);
      
      setNumero("");
      setCliente("");
      setValor("");
      
    } catch (error) {
      console.error("Erro ao criar pedido", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <NavLayout>
      <div className="container-fluid py-4" style={{ minHeight: "90vh" }}>
        <PageHeader
          title="Criar Pedido"
          subtitle="Preencha os dados abaixo para cadastrar um novo pedido"
        />

        <div className="row justify-content-center">
          <div className="col-12 col-md-8 col-lg-6">
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label className="form-label fw-semibold">
                  Número do Pedido
                </label>
                <input
                  type="text"
                  className="form-control form-control-lg"
                  placeholder="Ex: PED-001"
                  value={numero}
                  onChange={(e) => setNumero(e.target.value)}
                  disabled={loading}
                  required
                />
              </div>

              <div className="mb-3">
                <label className="form-label fw-semibold">
                  Nome do Cliente
                </label>
                <input
                  type="text"
                  className="form-control form-control-lg"
                  placeholder="Ex: João Silva"
                  value={cliente}
                  onChange={(e) => setCliente(e.target.value)}
                  disabled={loading}
                  required
                />
              </div>

              <div className="mb-3">
                <label className="form-label fw-semibold">
                  Valor (R$)
                </label>
                <input
                  type="number"
                  step="0.01" // Permite decimais
                  className="form-control form-control-lg"
                  placeholder="Ex: 150.00"
                  value={valor}
                  onChange={(e) => setValor(e.target.value)}
                  disabled={loading}
                  required
                />
              </div>

              <div className="d-flex justify-content-end mt-4">
                <button 
                  type="submit" 
                  className="btn btn-primary px-4 d-flex align-items-center"
                  disabled={loading}
                >
                  {loading && (
                    <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                  )}
                  {loading ? "Salvando..." : "Salvar Pedido"}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </NavLayout>
  );
}