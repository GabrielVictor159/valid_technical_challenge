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
        <img src={collapsed ? '/caret-right-square-fill.svg' : '/caret-left-square-fill.svg'} />
      </button>

      {!collapsed && <h4 className="text-white">Loja</h4>}
      <hr />

      <ul className="nav nav-pills flex-column mb-auto">
        <li className="nav-item">
          <Link
            to="/orders"
            className={`nav-link text-white d-flex align-items-center ${isActive("/orders")}`}
          >
            <img
              src="/clipboard2-data-fill.svg"
              style={{ filter: 'brightness(0) invert(1)' }}
            />
            {!collapsed && <span className="ms-2">Pedidos</span>}
          </Link>
        </li>

        <li>
          <Link
            to="/create-order"
            className={`nav-link text-white d-flex align-items-center ${isActive("/create-order")}`}
          >
            <img
              src="clipboard2-plus-fill.svg"
              style={{ filter: 'brightness(0) invert(1)' }} />
            {!collapsed && <span className="ms-2">Criar Pedidos</span>}
          </Link>
        </li>
      </ul>
    </div>
  );
}