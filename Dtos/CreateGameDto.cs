using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos;
/*
    Data annotation states what is required from the DTOs.
*/
public record class CreateGameDto(
    [Required][StringLength(50)] string Name,
    int GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
    );