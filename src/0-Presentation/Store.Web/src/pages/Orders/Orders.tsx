import NavLayout from "@/components/NavLayout/NavLayout";
import OrdersTable from "@/components/OrdersTable/OrdersTable";
import PageHeader from "@/components/PageHeader/PageHeader";

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
