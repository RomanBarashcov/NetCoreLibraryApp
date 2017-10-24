export class BookViewModel {
    constructor(
        public id: string,
        public year: number,
        public name: string,
        public description: string,
        public authorId: string,
        public authorName: string,
        public  authorSurname: string
    ) { }

}