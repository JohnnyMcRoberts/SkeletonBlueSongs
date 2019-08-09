import { IDbElement } from './IDbElement';

export interface IAlbumPlayed extends IDbElement
{
    date: Date;
    location: string;
    artist: string;
    album: string;
    userName: string;
    imagePath: string;
    playerLink: string;
};


export class AlbumPlayed implements IAlbumPlayed
{
    static fromData(data: IAlbumPlayed)
    {
        return new this(
            data._id,
            data.date,
            data.location,
            data.artist,
            data.album,
            data.userName,
            data.imagePath,
            data.playerLink);
    }

    constructor(
        public _id: string = "",
        public date: Date = new Date(),
        public location: string = "",
        public artist: string = "",
        public album: string = "",
        public userName: string = "",
        public imagePath: string = "",
        public playerLink: string = "")
    {
    }
};