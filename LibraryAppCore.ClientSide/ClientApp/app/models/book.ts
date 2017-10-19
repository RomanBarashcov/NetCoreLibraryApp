export class Book {
    constructor(
        public id: string,
        public year: number,
        public name: string,
        public language: string,
        public binding: string,
        public weight: number,
        public pages: number,
        public subscription: string,
        public price: string,
        public description: string,
        public imageBook: any,
        public authorId: string

    ) { }

}