SELECT G.Description
FROM Genres G
WHERE G.Name != Z
AND NOT EXISTS
((SELECT Songs.AlbumId
  FROM Songs
  WHERE Songs.GenreId = G.Id)
 EXCEPT
 (SELECT Songs.AlbumId
  FROM Songs
  WHERE Songs.GenreId IN
  	(SELECT Genres.Id
 FROM Genres
 WHERE Genres.Name = Z)))
AND NOT EXISTS
((SELECT Songs.AlbumId
  FROM Songs
  WHERE Songs.GenreId IN
  	(SELECT Genres.Id
 FROM Genres
 WHERE Genres.Name= Z))
 EXCEPT
 (SELECT Songs.AlbumId
  FROM Songs
  WHERE Songs.GenreId = G.Id));
