import { Author } from "./author";

export class AuthorPagedResult {
    constructor(
        public tageNumber: number,
        public pageSize: number,
        public totalNumberOfPages: any[],
        public totalNumberOfRecords: number,
        public results: Author[]
    ) { }
}