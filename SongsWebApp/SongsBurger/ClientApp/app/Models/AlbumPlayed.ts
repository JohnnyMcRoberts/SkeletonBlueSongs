import { IDbElement } from './IDbElement';

export interface IAlbumPlayed extends IDbElement
{
    date: Date;
    location: string;
    artist: string;
    album: string;
    user_name: string;
};


export class AlbumPlayed implements IAlbumPlayed
{
    constructor(
        public _id: string = "",
        public date: Date = new Date(),
        public location: string = "",
        public artist: string = "",
        public album: string = "",
        public user_name: string = "")
    {
    }
};