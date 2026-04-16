import { defineConfig } from 'vite'
import react, { reactCompilerPreset } from '@vitejs/plugin-react'
import babel from '@rolldown/plugin-babel'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    babel({ presets: [reactCompilerPreset()] })
  ],
  server: {
    port: 8080, 
    strictPort: true, 
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src')
    }
  },
  envDir: path.resolve(__dirname, './env'),
})
