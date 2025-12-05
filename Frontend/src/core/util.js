// In production mode, Rspack automatically replace process.env.NODE_ENV in code with 'production'.
const isDev = process.env.NODE_ENV === "development";

export { isDev };
