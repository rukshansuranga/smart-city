import { Label, HelperText } from "flowbite-react";
import ReactDatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import {
  Control,
  FieldValues,
  UseControllerProps,
  useController,
} from "react-hook-form";

type Props<T extends FieldValues> = {
  label?: string;
  placeholder?: string;
  value?: Date;
  showlabel?: boolean;
  className?: string;
  required?: boolean;
  control: Control<T>;
} & UseControllerProps<T>;

const DateField = <T extends FieldValues>(props: Props<T>) => {
  const { fieldState, field } = useController({ ...props });

  return (
    <div className="w-full">
      {props?.showlabel && (
        <div className="mt-2 mb-1 block">
          <Label htmlFor={props.name}>{props.label}</Label>
          {props.required && <span className="text-red-600">*</span>}
        </div>
      )}
      <ReactDatePicker
        className={
          "w-full px-3 py-2 rounded border border-gray-300 focus:border-blue-500 focus:outline-none " +
          (props.className || "")
        }
        selected={field.value}
        onChange={(date) => field.onChange(date)}
        placeholderText={props.placeholder}
        portalId="root-portal"
        dateFormat="yyyy-MM-dd"
        showPopperArrow={false}
        // Pass any additional props as needed
      />
      <HelperText>{fieldState.error?.message}</HelperText>
    </div>
  );
};
export default DateField;
