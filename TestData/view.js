[
  {
    $project: {
      _id: 1,
      oppDetails: "$$ROOT"
    }
  },
  {
    $lookup:
      {
        from: "metadata",
        localField: "_id",
        foreignField: "oppId",
        as: "oppMetadata"
      }
  }
]