export interface PaginationResponse<T> {
    items: T[];
    pageIndex: number;
    pageSize: number;
}

export interface PaginationRequest {
    pageIndex: number;
    pageSize: number;
}