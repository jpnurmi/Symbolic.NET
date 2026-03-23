use std::ffi::{CStr, CString};
use std::os::raw::c_char;

use symbolic_demangle::demangle;

#[no_mangle]
pub unsafe extern "C" fn symbolic_demangle_symbol(input: *const c_char) -> *mut c_char {
    if input.is_null() {
        return std::ptr::null_mut();
    }

    let c_str = unsafe { CStr::from_ptr(input) };
    let input_str = match c_str.to_str() {
        Ok(s) => s,
        Err(_) => return std::ptr::null_mut(),
    };

    let result = demangle(input_str);
    if result == input_str {
        return std::ptr::null_mut();
    }

    match CString::new(result.into_owned()) {
        Ok(s) => s.into_raw(),
        Err(_) => std::ptr::null_mut(),
    }
}

#[no_mangle]
pub unsafe extern "C" fn symbolic_demangle_free(s: *mut c_char) {
    if !s.is_null() {
        unsafe {
            drop(CString::from_raw(s));
        }
    }
}
