SELECT p.CategoryId, Count(*) as ProductsCount
FROM StockItem AS S
INNER JOIN ProductCategories AS P ON P.CategoryId = S.CategoryId
GROUP BY P.CategoryId