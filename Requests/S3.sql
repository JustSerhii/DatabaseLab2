SELECT Artists.Name
FROM Artists
WHERE Artists.Id NOT IN
	(SELECT Albums.ArtistId
	 FROM Albums
	 WHERE Albums.Title = X);
