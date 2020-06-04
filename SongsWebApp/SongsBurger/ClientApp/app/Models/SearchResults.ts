export interface SearchAlbum
{
    mbid: string;
    name: string;
    artist: string;
    summary: string;
    image: string;
    url: string;
}

export interface SearchArtist 
{
    mbid: string;
    name: string;
    summary: string;
    listeners: number;
    playcount: number;
    url: string;
    image: string;
}