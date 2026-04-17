import type { CreateOrderRequest } from '@/types/order/CreateOrderRequest';
import api from './api';
import type { CreateOrderResponse } from '@/types/order/CreateOrderResponse';
import type { SearchOrderResponse } from '@/types/order/SearchOrderResponse';
import type { OrderSearchRequest } from '@/types/order/OrderSearchRequest';
import type { PagedResponse } from '@/types/common/PagedResponse';


const orderService = {
  create: async (data: CreateOrderRequest) => {
    const response = await api.post<CreateOrderResponse>('/api/orders', data);
    return response.data;
  },

  getById: async (id: number) => {
    const response = await api.get<SearchOrderResponse>(`/api/orders/${id}`);
    return response.data;
  },

  getPaged: async (params: OrderSearchRequest) => {
    const response = await api.get<PagedResponse<SearchOrderResponse>>('/api/orders', {
      params, 
    });
    return response.data;
  },
};

export default orderService;