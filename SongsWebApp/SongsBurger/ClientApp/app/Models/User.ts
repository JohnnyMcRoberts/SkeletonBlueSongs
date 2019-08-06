export interface IUserAddRequest
{
    name: string;
    password: string;
    description: string;
    email: string;
};

export class UserAddRequest implements IUserAddRequest
{
    static fromData(data: IUserAddRequest) {
        return new this(
            data.name,
            data.password,
            data.description,
            data.email);
    }

    constructor(
        public name: string = "",
        public password: string = "",
        public description: string = "",
        public email: string = "")
    {
    }
};

export interface IUserAddResponse {
    name: string;
    errorCode: number;
    failReason: string;
    userId: string;
};

export class UserAddResponse implements IUserAddResponse
{
    static fromData(data: IUserAddResponse)
    {
        return new this(
            data.name,
            data.errorCode,
            data.failReason,
            data.userId);
    }

    constructor(
        public name: string = "",
        public errorCode: number = -1,
        public failReason: string = "",
        public userId: string = "")
    {
    }
};
