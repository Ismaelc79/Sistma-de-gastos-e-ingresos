import { forwardRef, useState, useCallback } from 'react';
import type { InputHTMLAttributes } from 'react';

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  helperText?: string;
  showPasswordToggle?: boolean;
}

export const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, helperText, className = '', showPasswordToggle = false, type = 'text', ...props }, ref) => {
    const [showPassword, setShowPassword] = useState(false);

    const isPasswordType = type === 'password';
    const shouldShowToggle = showPasswordToggle && isPasswordType;

    const handleTogglePassword = useCallback((e: React.MouseEvent<HTMLButtonElement>) => {
      e.preventDefault();
      e.stopPropagation();
      setShowPassword(prev => !prev);
    }, []);

    const inputType = shouldShowToggle ? (showPassword ? 'text' : 'password') : type;

    return (
      <div className="w-full">
        {label && (
          <label className="block text-sm font-medium text-dark-700 mb-1">
            {label}
            {props.required && <span className="text-red-500 ml-1">*</span>}
          </label>
        )}
        <div className="relative">
          <input
            ref={ref}
            type={inputType}
            className={`
              w-full px-4 py-2 pr-10
              border rounded-lg
              focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent
              disabled:bg-dark-100 disabled:cursor-not-allowed
              transition-all duration-200
              ${error ? 'border-red-500' : 'border-dark-300'}
              ${className}
            `}
            {...props}
          />
          {shouldShowToggle && (
            <button
              type="button"
              className="absolute inset-y-0 right-0 pr-3 flex items-center cursor-pointer hover:bg-gray-50 rounded-r-lg transition-colors"
              onClick={handleTogglePassword}
              tabIndex={-1}
              style={{ pointerEvents: 'auto' }}
            >
              {showPassword ? (
                <svg className="h-5 w-5 text-dark-400 hover:text-dark-600 transition-colors" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.878 9.878L3 3m6.878 6.878L12 12m3.121 3.121L21 21m-6.878-6.878l-3.121-3.121" />
                </svg>
              ) : (
                <svg className="h-5 w-5 text-dark-400 hover:text-dark-600 transition-colors" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
              )}
            </button>
          )}
        </div>
        {error && (
          <p className="mt-1 text-sm text-red-500">{error}</p>
        )}
        {helperText && !error && (
          <p className="mt-1 text-sm text-dark-500">{helperText}</p>
        )}
      </div>
    );
  }
);

Input.displayName = 'Input';
