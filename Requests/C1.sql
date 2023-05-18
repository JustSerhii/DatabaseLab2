SELECT Labels.Name
FROM Labels
WHERE Labels.Id IN
	(SELECT Artists.LabelId
	 FROM Artists
	 WHERE Artists.Id IN
		(SELECT A.Id
		 FROM Artists A
		 WHERE NOT EXISTS
	 		((SELECT Albums.Price
			  FROM Albums
		      WHERE Albums.ArtistId = K)
		     EXCEPT
		     (SELECT Albums.Price
		      FROM Albums
		      WHERE Albums.ArtistId = A.id AND Albums.ArtistId != K))));