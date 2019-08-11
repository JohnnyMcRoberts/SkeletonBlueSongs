import { IAlbumPlayed, AlbumPlayed } from './AlbumPlayed';

export interface ISongsFilesDetailsRequest
{
    fileName: string;
    filePath: string;
};

export class SongsFilesDetailsRequest implements ISongsFilesDetailsRequest
{
    static fromData(data: ISongsFilesDetailsRequest)
    {
        return new this(
            data.fileName,
            data.filePath);
    }

    constructor(
        public fileName: string = "",
        public filePath: string = "")
    {
    }
};

export interface ISongsFilesDetailsResponse
{
    fileName: string;
    errorCode: number;
    failReason: string;
    albumsPlayed: IAlbumPlayed[];
};


export class SongsFilesDetailsResponse implements ISongsFilesDetailsResponse
{
    static fromData(data: ISongsFilesDetailsResponse)
    {
        var albumsPlayed: IAlbumPlayed[] = new Array<AlbumPlayed>();

        for (var i = 0; i < data.albumsPlayed.length; i++)
        {
            albumsPlayed.push(data.albumsPlayed[i]);
        }

        return new this(
            data.fileName,
            data.errorCode,
            data.failReason,
            albumsPlayed);
    }

    constructor(
        public fileName: string = "",
        public errorCode: number = 0,
        public failReason: string = "",
        public albumsPlayed: IAlbumPlayed[] = new Array<AlbumPlayed>())
    {
    }
};

export interface ISongsValuesDetails
{
    locationValues: string[];
    artistValues: string[];
    albumValues: string[];
};

export class SongsValuesDetails implements ISongsValuesDetails
{
    static fromData(data: ISongsValuesDetails)
    {
        return new this(
            data.locationValues,
            data.artistValues,
            data.albumValues);
    }

    constructor(
        public locationValues: string[] = new Array<string>(),
        public artistValues: string[] = new Array<string>(),
        public albumValues: string[] = new Array<string>())
    {
    }
};


