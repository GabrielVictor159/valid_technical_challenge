import NavLayout from "@/components/NavLayout";
import OrdersTable from "@/components/OrdersTable";
import PageHeader from "@/components/PageHeader";

export default function Orders() {
  return (
    <>
      <NavLayout>
        <div className="container-fluid py-4" style={{ minHeight: "90vh" }}>
          <PageHeader title="Pedidos"/>
          <OrdersTable />
        </div>
      </NavLayout>
    </>
  );
}
