import { useState, type ReactNode } from "react";
import Sidebar from "@/components/Sidebar";

interface NavPageProps {
  children: ReactNode;
}

export default function NavLayout({ children }: NavPageProps) {
  const [collapsed, setCollapsed] = useState(true);

  return (
    <div>
      <Sidebar
        collapsed={collapsed}
        toggle={() => setCollapsed(!collapsed)}
      />

      <div
        style={{
          marginLeft: collapsed ? "80px" : "250px",
          padding: "20px",
          transition: "0.3s",
        }}
      >
        {children}
      </div>
    </div>
  );
}