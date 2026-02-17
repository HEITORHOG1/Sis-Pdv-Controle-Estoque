window.onload = function () {
    // Get the route prefix from the current URL
    var routePrefix = '/api-docs';
    
    // Initialize Swagger UI
    const ui = SwaggerUIBundle({
        url: routePrefix + "/v1/swagger.json",
        dom_id: '#swagger-ui',
        deepLinking: true,
        presets: [
            SwaggerUIBundle.presets.apis,
            SwaggerUIStandalonePreset
        ],
        plugins: [
            SwaggerUIBundle.plugins.DownloadUrl
        ],
        layout: "StandaloneLayout",
        validatorUrl: null,
        defaultModelsExpandDepth: 1,
        defaultModelExpandDepth: 1,
        displayOperationId: false,
        displayRequestDuration: true,
        docExpansion: 'list',
        filter: true,
        showExtensions: true,
        showCommonExtensions: true,
        syntaxHighlight: {
            activate: true,
            theme: "monokai"
        },
        tryItOutEnabled: true,
        requestSnippetsEnabled: true,
        requestSnippets: {
            generators: {
                "curl_bash": {
                    title: "cURL (bash)",
                    syntax: "bash"
                },
                "curl_powershell": {
                    title: "cURL (PowerShell)",
                    syntax: "powershell"
                },
                "curl_cmd": {
                    title: "cURL (CMD)",
                    syntax: "bash"
                }
            },
            defaultExpanded: true,
            languages: null
        }
    });

    window.ui = ui;
};
