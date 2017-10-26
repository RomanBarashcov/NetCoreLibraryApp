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
    public class DocumentRepository : IDocumentRepository
    {
        private List<BookPostgreSql> bookList = new List<BookPostgreSql>();
        private IAuthorRepository authorRepo;
        private IBookRepository bookRepo;

        public DocumentRepository(IAuthorRepository _authorRepository, IBookRepository _bookRepository)
        {
            authorRepo = _authorRepository;
            bookRepo = _bookRepository;
        }

        public List<BookPostgreSql> ReadDocument(IFormFile file)
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
                    LoadDataFromColumn(sheet);
                }
            }

            return bookList;
        }

        private void LoadDataFromColumn(ISheet sheet)
        {
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                BookPostgreSql book = new BookPostgreSql();

                try { book.Year = Convert.ToInt32(sheet.GetRow(row).GetCell(0).NumericCellValue); }
                catch { book.Year = 0; }

                book.Name = sheet.GetRow(row).GetCell(1).StringCellValue;
                book.Description = sheet.GetRow(row).GetCell(2).StringCellValue;

                try { book.AuthorId = Convert.ToInt32(sheet.GetRow(row).GetCell(3).NumericCellValue); }
                catch { book.AuthorId = 0; }

                bookList.Add(book);
            }
        }

        public async Task<bool> SaveData(List<BookPostgreSql> books)
        {
            bool operationSuccess = false;

            if(books != null)
            {
                foreach(BookPostgreSql b in books)
                {
                    Book book = new Book();

                    book.AuthorId = Convert.ToString(b.AuthorId);
                    book.Year = b.Year;
                    book.Name = b.Name;
                    book.Description = b.Description;

                    int dbresult = await bookRepo.CreateBook(book);

                    if(dbresult == 1)
                    {
                        operationSuccess = true;
                    }
                }
            }

            return operationSuccess;
        }
    }
}
