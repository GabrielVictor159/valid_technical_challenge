import { createBrowserRouter, Navigate } from "react-router-dom";
import Orders from "@/pages/Orders/Orders";
import OrdersDetails from "@/pages/OrderDetails/OrderDetails";
import { ProtectedRoute } from "./ProtectedRoute";
import CreateOrder from "@/pages/CreateOrder/CreateOrder";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Navigate to="/create-order" replace />
  },
  {
    element: <ProtectedRoute />, 
    children: [
      {
        path: '/orders',
        element: <Orders />
      },
    ]
  },
  {
    element: <ProtectedRoute />, 
    children: [
      {
        path: '/order-details/:id',
        element: <OrdersDetails />
      },
    ]
  },
  {
    element: <ProtectedRoute />, 
    children: [
      {
        path: '/create-order',
        element: <CreateOrder />
      },
    ]
  },
  {
    path: '/unauthorized',
    element: <div>Você não tem permissão para acessar esta página.</div>
  }
]);