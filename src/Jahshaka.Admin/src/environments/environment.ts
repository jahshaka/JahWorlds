// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
    production: false,
    // resourceServerUrl: 'http://localhost:6001',
    resourceServerUrl: 'http://api.dev.jahfx.com',
    // authServerUrl: 'http://localhost:5000',
    authServerUrl: 'http://auth.dev.jahfx.com',
    clientId: 'admin',
    clientSecret: 'secret_secret_secret',
    grantType: 'password'
};
