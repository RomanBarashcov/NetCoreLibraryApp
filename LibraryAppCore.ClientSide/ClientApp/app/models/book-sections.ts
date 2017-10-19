export class BookSections {
    constructor(
        public Id: string,
        public New: boolean,
        public ForFamily: boolean,
        public Technical: boolean, 
        public Fiction: boolean,
        public ForBusness: boolean,
        public BookId: string
    ) {}

}