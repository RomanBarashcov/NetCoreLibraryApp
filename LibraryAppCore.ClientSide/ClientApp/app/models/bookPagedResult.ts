import { Book } from './book';

export class BookPagedResult {
    constructor(
        public pageNumber: number,
        public pageSize: number,
        public totalNumberOfPages: any[],
        public totalNumberOfRecords: number,
        public results: Book[]
    ) { }
}