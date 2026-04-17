import axios from 'axios';
import { store } from '@/store';
import { toast } from 'react-toastify'; 
import keycloak from '@/services/keycloak';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:8181',
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  const state = store.getState();
  const token = state.auth.token;
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    const responseData = error.response?.data;
    console.log('responseData', responseData)
    console.log('error response', error.response)
    if (error.response?.status === 400) {
      if (responseData.errors && responseData.errors.length > 0) {
        responseData.errors.forEach((err: string) => {
          console.log('tost', err)
          toast.error(err);
        });
      } else if (responseData.message) {
        toast.error(responseData.message);
      }
    } else if (error.response?.status === 401) {
       keycloak.login();
    } else if (error.response?.status === 500) {
       toast.error("Erro interno no servidor. Tente novamente mais tarde.");
    }

    return Promise.reject(error);
  }
);

export default api;