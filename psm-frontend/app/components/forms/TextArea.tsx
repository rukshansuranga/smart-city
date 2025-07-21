import { Label, HelperText, Textarea } from "flowbite-react";
import {
  UseControllerProps,
  useController,
  FieldValues,
  Control,
} from "react-hook-form";

type Props<T extends FieldValues> = {
  label?: string;
  placeholder?: string;
  rows?: number;
  showlabel?: boolean;
  className?: string;
  required?: boolean;
  control: Control<T>;
} & UseControllerProps<T>;

const TextAreaField = <T extends FieldValues>(props: Props<T>) => {
  const { fieldState, field } = useController({ ...props });

  return (
    <div className="w-full">
      {props?.showlabel && (
        <div className="mt-2 mb-1 block">
          <Label htmlFor={props.name}>{props.label}</Label>
          {props.required && <span className="text-red-600">*</span>}
        </div>
      )}
      <Textarea
        {...props}
        {...field}
        placeholder={props.placeholder}
        className={props.className}
        rows={props.rows ?? 4}
        color={
          fieldState.error ? "failure" : !fieldState.isDirty ? "" : "success"
        }
      />
      <HelperText>{fieldState.error?.message}</HelperText>
    </div>
  );
};
export default TextAreaField;
