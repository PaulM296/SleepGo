export interface PaginationResponse<T> {
    items: T[];
    pageIndex: number;
    totalPages: number;
}

export interface PaginationRequest {
    pageIndex: number;
    pageSize: number;
}