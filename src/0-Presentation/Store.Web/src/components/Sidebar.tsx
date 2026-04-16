import { Link, useLocation } from "react-router-dom";

interface SidebarProps {
  collapsed: boolean;
  toggle: () => void;
}

export default function Sidebar({ collapsed, toggle }: SidebarProps) {
  const location = useLocation();

  const isActive = (path: string) =>
    location.pathname === path ? "active" : "";

  return (
    <div
      className="d-flex flex-column flex-shrink-0 p-3 text-bg-dark"
      style={{
        width: collapsed ? "80px" : "250px",
        height: "100vh",
        position: "fixed",
        transition: "0.3s",
      }}
    >
      <button
        type="button"
        className="btn btn-sm btn-light mb-3"
        onClick={toggle}
        >
        {collapsed ? "➡️" : "⬅️"}
        </button>

      {!collapsed && <h4 className="text-white">Loja</h4>}
      <hr />

      <ul className="nav nav-pills flex-column mb-auto">
        <li className="nav-item">
          <Link
            to="/orders"
            className={`nav-link text-white d-flex align-items-center ${isActive("/orders")}`}
          >
            <span>📋</span>
            {!collapsed && <span className="ms-2">Pedidos</span>}
          </Link>
        </li>

        <li>
          <Link
            to="/create-order"
            className={`nav-link text-white d-flex align-items-center ${isActive("/create-order")}`}
          >
            <span>📝</span>
            {!collapsed && <span className="ms-2">Criar Pedidos</span>}
          </Link>
        </li>
      </ul>
    </div>
  );
}