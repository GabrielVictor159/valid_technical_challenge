import Sidebar from "@/components/Sidebar/Sidebar";
import { ToastContainer } from "react-toastify";
import { useSelector, useDispatch } from "react-redux";
import { type RootState } from "@/store/index"; 
import { toggleSidebar } from "@/features/ui/uiSlice";
import type { ReactNode } from "react";

interface NavPageProps {
  children: ReactNode;
}

export default function NavLayout({ children }: NavPageProps) {
  const dispatch = useDispatch();
  const collapsed = useSelector((state: RootState) => state.ui.sidebarCollapsed);

  return (
    <>
      <div>
        <Sidebar
          collapsed={collapsed}
          toggle={() => dispatch(toggleSidebar())}
        />

        <div
          style={{
            marginLeft: collapsed ? "80px" : "250px",
            padding: "20px",
            transition: "0.3s",
            minHeight: "100vh"
          }}
        >
          {children}
        </div>
      </div>
      <ToastContainer />
    </>
  );
}