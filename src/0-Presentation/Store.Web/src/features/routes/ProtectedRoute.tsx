import { useEffect } from 'react';
import { Outlet } from 'react-router-dom';
import keycloak from '@/services/keycloak';

export const ProtectedRoute = () => {
  useEffect(() => {
    if (!keycloak.authenticated) {
      keycloak.login();
    }
  }, []);

  if (!keycloak.authenticated) {
    return <div>Carregando autenticação...</div>;
  }

  return <Outlet />;
};