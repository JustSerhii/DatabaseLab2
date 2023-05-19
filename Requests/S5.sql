SELECT Genres.Name
FROM Genres
WHERE Genres.id IN
	(SELECT Songs.GenreId
	 FROM Songs
	 WHERE Songs.AlbumId IN
	 	(SELECT Albums.Id
		 FROM Albums
		 WHERE Albums.ArtistId IN
		 	(SELECT Artists.id
			 FROM Artists
			 WHERE Artists.Name = X)));