import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import { fileURLToPath, URL } from 'node:url';
// In dev, requests to /api are proxied to the backend HTTPS endpoint so we avoid
// CORS/self-signed-cert friction. Override the target with VITE_DEV_API_TARGET.
export default defineConfig(function (_a) {
    var mode = _a.mode;
    var env = loadEnv(mode, process.cwd(), '');
    var devApiTarget = env.VITE_DEV_API_TARGET || 'https://localhost:7133';
    return {
        plugins: [react()],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url)),
            },
        },
        server: {
            port: 5173,
            proxy: {
                '/api': {
                    target: devApiTarget,
                    changeOrigin: true,
                    secure: false,
                },
            },
        },
    };
});
