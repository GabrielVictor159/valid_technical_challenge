import type { SearchOperationResponse } from "../operation/SearchOperationResponse";

export interface SearchOrderResponse {
  id: number;
  numberOrder: string;
  status: number;
  createdDate: string;
  totalPrice: number;
  operations: SearchOperationResponse[];
}