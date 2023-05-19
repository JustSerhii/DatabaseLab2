SELECT AVG(Albums.Price)
FROM Albums
WHERE Albums.ArtistId IN
	(SELECT Artists.Id
	 FROM Artists
	 WHERE Artists.Name = Z);

