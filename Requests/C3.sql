SELECT G.Name
FROM Genres G
WHERE G.Description LIKE Y
AND NOT EXISTS
	((SELECT Songs.AlbumId
	  FROM Songs
	  WHERE Songs.GenreId = G.Id)
	 EXCEPT
	 (SELECT Albums.Id
	  FROM Albums))
AND NOT EXISTS
	((SELECT Albums.Id
	  FROM Albums)
	 EXCEPT
	 (SELECT Songs.AlbumId
	  FROM Songs
	  WHERE Songs.GenreId = G.Id));