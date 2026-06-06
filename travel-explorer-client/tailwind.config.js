/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        // Anthropic-inspired clay / terracotta accent (drop-in replacement for the
        // previous blue "brand" scale, so the whole app rebrands consistently).
        brand: {
          50: '#fbf4f0',
          100: '#f6e4d9',
          200: '#eec8b5',
          300: '#e3a585',
          400: '#db8a64',
          500: '#d97757',
          600: '#c0573a',
          700: '#9f4730',
          800: '#813c2b',
          900: '#6a3326',
        },
        // Warm "ivory canvas" neutrals.
        cream: {
          50: '#faf9f5',
          100: '#f4f0e8',
          200: '#ebe5d9',
          300: '#ddd4c3',
          400: '#c9bca5',
        },
        // Warm slate "ink" neutrals.
        ink: {
          50: '#e8e6dc',
          100: '#d8d6cd',
          200: '#b0aea5',
          300: '#8e8b82',
          400: '#6c6a64',
          500: '#514f4a',
          600: '#3d3d3a',
          700: '#2b2a27',
          800: '#1f1e1c',
          900: '#141413',
        },
      },
      fontFamily: {
        sans: ['Inter', 'system-ui', 'sans-serif'],
        serif: ['Fraunces', 'Georgia', 'Times New Roman', 'serif'],
        mono: ['"JetBrains Mono"', 'ui-monospace', 'SFMono-Regular', 'monospace'],
      },
      letterSpacing: {
        eyebrow: '0.22em',
      },
      boxShadow: {
        soft: '0 1px 2px rgba(20,20,19,0.04), 0 8px 24px -12px rgba(20,20,19,0.18)',
        lift: '0 24px 60px -28px rgba(20,20,19,0.35)',
      },
      keyframes: {
        'fade-up': {
          '0%': { opacity: '0', transform: 'translateY(20px)' },
          '100%': { opacity: '1', transform: 'translateY(0)' },
        },
        'fade-in': {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' },
        },
        float: {
          '0%, 100%': { transform: 'translateY(0)' },
          '50%': { transform: 'translateY(-14px)' },
        },
        'float-slow': {
          '0%, 100%': { transform: 'translateY(0) rotate(0deg)' },
          '50%': { transform: 'translateY(-22px) rotate(2deg)' },
        },
        marquee: {
          '0%': { transform: 'translateX(0)' },
          '100%': { transform: 'translateX(-50%)' },
        },
        shimmer: {
          '0%': { backgroundPosition: '-200% 0' },
          '100%': { backgroundPosition: '200% 0' },
        },
        'spin-slow': {
          '0%': { transform: 'rotate(0deg)' },
          '100%': { transform: 'rotate(360deg)' },
        },
      },
      animation: {
        'fade-up': 'fade-up 0.8s cubic-bezier(0.22,1,0.36,1) both',
        'fade-in': 'fade-in 0.9s ease both',
        float: 'float 6s ease-in-out infinite',
        'float-slow': 'float-slow 9s ease-in-out infinite',
        marquee: 'marquee 38s linear infinite',
        shimmer: 'shimmer 2.4s linear infinite',
        'spin-slow': 'spin-slow 28s linear infinite',
      },
    },
  },
  plugins: [],
}
