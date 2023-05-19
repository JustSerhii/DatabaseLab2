SELECT Albums.Title, Albums.Price
FROM Albums
WHERE Albums.ArtistId IN
	(SELECT Artists.Id
	 FROM Artists
	 WHERE Artists.LabelId IN
	 	(SELECT Labels.Id
		 FROM Labels
		 WHERE Labels.Name = Z));

