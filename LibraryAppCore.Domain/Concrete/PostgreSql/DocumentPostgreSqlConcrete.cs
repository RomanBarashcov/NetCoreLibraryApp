using LibraryAppCore.Domain.Abstracts;
using LibraryAppCore.Domain.Entities;
using LibraryAppCore.Domain.Entities.MsSql;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LibraryAppCore.Domain.Concrete
{
    public class DocumentPostgreSqlConcrete : IDocumentRepository
    {
        private List<Book> bookList = new List<Book>();
        private IAuthorRepository authorRepo;
        private IBookRepository bookRepo;

        public DocumentPostgreSqlConcrete(IAuthorRepository _authorRepository, IBookRepository _bookRepository)
        {
            authorRepo = _authorRepository;
            bookRepo = _bookRepository;
        }

        public async Task<List<Book>> ReadDocumentAsync(IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                ISheet sheet; //Create the ISheet object to read the sheet cell values  

                string filename = file.Name; //get the uploaded file name  
                var fileExt = Path.GetExtension(filename); //get the extension of uploaded excel file  
                if (fileExt == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //HSSWorkBook object will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //XSSFWorkBook will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
                }

                if (sheet.GetRow(0) != null) //null is when the row only contains empty cells   
                {
                    await LoadDataFromColumnAsync(sheet);
                }
            }

            return bookList;
        }

        private async Task LoadDataFromColumnAsync(ISheet sheet)
        {
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                BookPostgreSql bookPostgreSql = new BookPostgreSql();

                if(sheet.GetRow(row).GetCell(0).CellType == CellType.Numeric)
                {
                    bookPostgreSql.Year = Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue); 
                }
                
                if(sheet.GetRow(row).GetCell(1).CellType == CellType.String)
                {
                    bookPostgreSql.Name = sheet.GetRow(row).GetCell(1).StringCellValue;
                }

                if(sheet.GetRow(row).GetCell(2).CellType == CellType.String)
                {
                    bookPostgreSql.Description = sheet.GetRow(row).GetCell(2).StringCellValue;
                }
                    
                if (sheet.GetRow(row).GetCell(3).CellType == CellType.Numeric)
                {
                    bookPostgreSql.AuthorId = Convert.ToInt32(sheet.GetRow(row).GetCell(3).NumericCellValue);
                }
                else if(sheet.GetRow(row).GetCell(3).CellType == CellType.String)
                {
                    string authorName = sheet.GetRow(row).GetCell(3).StringCellValue;

                    string[] words = authorName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string firstName = words[0];
                    string surName = words[1];

                    string authorId = await authorRepo.GetAuthorIdByName(firstName, surName);

                    if(!String.IsNullOrEmpty(authorId))
                    {
                        bookPostgreSql.AuthorId = Convert.ToInt32(authorId);
                    }
                    else
                    {
                        Author author = new Author { Name = firstName, Surname = surName };
                        int dbResult = await authorRepo.CreateAuthor(author);

                        if (dbResult > 0)
                            bookPostgreSql.AuthorId = Convert.ToInt32(await authorRepo.GetAuthorIdByName(author.Name, author.Surname));

                    }
           
                }

                Book book = new Book(bookPostgreSql);

                bookList.Add(book);
            }
        }

        public async Task<bool> SaveData(List<Book> books)
        {
            bool operationSuccess = false;
            int operationSuccessCount = 0;

            if(books != null)
            {
                foreach(Book b in books)
                {
                    Book book = new Book();

                    book.AuthorId = b.AuthorId;
                    book.Year = b.Year;
                    book.Name = b.Name;
                    book.Description = b.Description;

                    int dbresult = await bookRepo.CreateBook(book);

                    if(dbresult == 1)
                    {
                        operationSuccessCount = + 1;
                    }
                }
            }

            if (operationSuccessCount > 0)
                operationSuccess = true;

            return operationSuccess;
        }
    }
}
