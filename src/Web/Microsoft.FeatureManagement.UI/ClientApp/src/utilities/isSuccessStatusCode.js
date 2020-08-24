export const isSuccessStatusCode = statusCode => !statusCode ? false : statusCode >= 200 && statusCode <= 299;

