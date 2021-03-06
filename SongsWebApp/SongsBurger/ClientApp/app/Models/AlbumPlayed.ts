﻿import { IDbElement } from './IDbElement';

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
            data.id,
            data.date,
            data.location,
            data.artist,
            data.album,
            data.userName,
            data.imagePath,
            data.playerLink);
    }

    constructor(
        public id: string = "",
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


export interface IAlbumPlayedAddResponse
{
    album: IAlbumPlayed;
    errorCode: number;
    failReason: string;
    userId: string;
};

export class AlbumPlayedAddResponse implements IAlbumPlayedAddResponse
{
    static fromData(data: IAlbumPlayedAddResponse)
    {
        var album: AlbumPlayed = AlbumPlayed.fromData(data.album);
        return new this(
            album,
            data.errorCode,
            data.failReason,
            data.userId);
    }

    constructor(
        public album: IAlbumPlayed = new AlbumPlayed(),
        public errorCode: number = -1,
        public failReason: string = "",
        public userId: string = "") {
    }
};

